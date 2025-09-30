import { MoverItemDto, SaveCardDto } from './../../../types/kanban';
import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BehaviorSubject, Subject} from 'rxjs';
import {KabanCardsDto, KanbanCardType, KanbanListType, KanbanListTypeMs} from '@/types/kanban';

@Injectable()
export class KanbanService {
    private _lists: KanbanListType[] = [];

    private newLists: KanbanListTypeMs[] = []

    private selectedCard = new Subject<KanbanCardType>();

    private selectedListId = new Subject<string>();

    private lists = new BehaviorSubject<KanbanListType[]>(this._lists);

    private listNames = new Subject<any[]>();

    lists$ = this.lists.asObservable();

    selectedCard$ = this.selectedCard.asObservable();

    selectedListId$ = this.selectedListId.asObservable();

    listNames$ = this.listNames.asObservable();

    baseUrl = 'http://localhost:5057/api'
    //baseUrl = 'http://localhost:7065/api'

    private listIdMap = new Map<string, string>();

    constructor(private http: HttpClient) {
        this.getLists();
    }

    private updateLists(data: any[]) {
        this._lists = data;
        let small = data.map((l) => ({ listId: l.listId, title: l.title }));

        this.listNames.next(small);
        this.lists.next([...data]);
    }
    private fromDto = (x: KanbanListTypeMs): KanbanListType => ({
      listId: x.code ?? String(x.id),
      title: x.title,
      cards: []
    });


    getLists(){
      this.http
        .get<KanbanListTypeMs[]>(`${this.baseUrl}/catalog/kanban-lists`)
        .subscribe(dto => {
          const ordered = [...dto].sort((a, b) => a.position - b.position);
          const ui = ordered.map(this.fromDto);

          this.listIdMap.clear();
          for(const row of ordered){
            const uiId = row.code ?? String(row.id);
            this.listIdMap.set(String(row.id), uiId);
          }
          this.updateLists(ui);

          for(const row of ordered) {
            this.getCardsByList(row.id)
          }
        });
    }

    private toUsernames(u: unknown): string[] {
      if (Array.isArray(u)) return u as string[];
      if (typeof u === 'string') {
        return u.split(/[;,]/).map(s => s.trim()).filter(Boolean);
      }
      return [];
    }

    private fromCardDto = (c: KabanCardsDto): KanbanCardType => {
      const usernames = this.toUsernames(c.asignadoAUsername);

      return {
        id: String(c.id),
        title: c.titulo ?? '',
        description: c.descripcion ?? '',
        progress: c.completed ? 100 : 0,
        assignees: usernames.map(name => ({ name, image: '' })),
        attachments: c.attachments ?? 0,
        comments: Array.isArray(c.comments) ? c.comments : [],
        startDate: c.startDate ?? '',
        dueDate: c.dueDate ?? '',
        completed: !!c.completed,
        priority: { severity: c.severidad },
        taskList: { title: 'Tasks', tasks: [] }
      };
    };

    getCardsByList(idList: number) {
      this.http
        .get<KabanCardsDto[]>(`${this.baseUrl}/catalog/kanban-list-detail/$${idList}`)
        .subscribe(dto => {
          const uiListId = this.listIdMap.get(String(idList)) ?? String(idList);

          const cards: KanbanCardType[] = dto.map( d=> {
            const base = this.fromCardDto(d);
            return { ...base,id: `${uiListId}:${base.id}` };
          });

          const next = this._lists.map(l =>
            l.listId === uiListId ? {...l, cards: [...cards]} : l
          );

          this.updateLists(next);
        });
    }

    addList() {
        const listId = this.generateId();
        const title = 'Untitled List';
        const newList = {
            listId: listId,
            title: title,
            cards: []
        };

        this._lists.push(newList);
        this.lists.next(this._lists);
    }

    addCard(listId: string) {
        const cardId = this.generateId();
        const title = 'Untitled card';
        const newCard = { id: cardId, title: title, description: '', progress: '', assignees: [], attachments: 0, comments: [], startDate: '', dueDate: '', completed: false, taskList: { title: 'Untitled Task List', tasks: [] } };

        let lists = [];
        lists = this._lists.map((l) => (l.listId === listId ? { ...l, cards: [...(l.cards || []), newCard] } : l));
        this.updateLists(lists);
    }

    private toYmd(d?: string | Date): string {
      if (!d) return '';
      const date = typeof d === 'string' ? new Date(d) : d;
      const y = date.getFullYear();
      const m = String(date.getMonth() + 1).padStart(2, '0');
      const day = String(date.getDate()).padStart(2, '0');
      return `${y}-${m}-${day}`;
    }

    private lastIdPiece(id: string): string {
      const parts = String(id).split(':');
      return parts[parts.length - 1];
    }

    private buildSaveDto(card: KanbanCardType, listId: string): SaveCardDto {
      const sev =
        (card as any)?.priority?.severity ?? 0;
      return {
        listCode: listId,
        titulo: card.title ?? '',
        descripcion: card.description ?? '',
        startDate: this.toYmd(card.startDate),
        dueDate: this.toYmd(card.dueDate),
        severidad: Number(sev) || 0,
        asignadoAUsernameId: 0,                       // TODO: mapear a un Id con el otro ms
        externalId: this.lastIdPiece(card.id)
      };
    }
    updateCard(card: KanbanCardType, listRef: string | number) {

      let listIndex = -1;
      let listId = '';

      if (typeof listRef === 'number') {
        listIndex = listRef;
        listId = this._lists[listIndex]?.listId ?? '';
      } else {
        listId = listRef;
        listIndex = this._lists.findIndex(l => l.listId === listId);
      }

      if (listIndex < 0 || !listId) {
        console.warn('No se pudo resolver índice/id de lista', { listRef, listIndex, listId });
        return;
      }

      const prev = this._lists.map(l => ({ ...l, cards: [...(l.cards ?? [])] }));
      const next = this._lists.map((l, i) =>
        i === listIndex
          ? { ...l, cards: l.cards.map(c => (c.id === card.id ? { ...card } : c)) }
          : l
      );
      this.updateLists(next);

      const toYmd = (d?: string | Date) => {
        if (!d) return '';
        const dt = typeof d === 'string' ? new Date(d) : d;
        const y = dt.getFullYear();
        const m = String(dt.getMonth() + 1).padStart(2, '0');
        const day = String(dt.getDate()).padStart(2, '0');
        return `${y}-${m}-${day}`;
      };

      const severity = (card as any)?.priority?.severity ?? 0;
      const payload = {
        listCode: listRef.toString(),
        titulo: card.title ?? '',
        descripcion: card.description ?? '',
        startDate: toYmd(card.startDate),
        dueDate: toYmd(card.dueDate),
        severidad: Number(severity) || 0,
        asignadoAUsernameId: 0,
        externalId: String(card.id).split(':').pop() ?? String(card.id),
      };

      this.http.post(`${this.baseUrl}/catalog/newItem`, payload).subscribe({
        next: () => { /* ok */ },
        error: err => {
          console.error('Error guardando tarjeta', err);
          this.updateLists(prev);
        }
      });
    }

    deleteList(id: string) {
        this._lists = this._lists.filter((l) => l.listId !== id);
        this.lists.next(this._lists);
    }

    copyList(list: KanbanListType) {
        let newId = this.generateId();
        let newList = { ...list, listId: newId };

        this._lists.push(newList);
        this.lists.next(this._lists);
    }

    deleteCard(cardId: string, listId: string) {
        let lists = [];

        for (let i = 0; i < this._lists.length; i++) {
            let list = this._lists[i];

            if (list.listId === listId && list.cards) {
                list.cards = list.cards.filter((c) => c.id !== cardId);
            }

            lists.push(list);
        }

        this.updateLists(lists);
    }

    copyCard(card: KanbanCardType, listId: string) {
        let lists = [];

        for (let i = 0; i < this._lists.length; i++) {
            let list = this._lists[i];

            if (list.listId === listId && list.cards) {
                let cardIndex = list.cards.indexOf(card);
                let newId = this.generateId();
                let newCard = { ...card, id: newId };
                list.cards.splice(cardIndex, 0, newCard);
            }

            lists.push(list);
        }

        this.updateLists(lists);
    }

    private uiToDbListId(uiId: string): number | null {
      for (const [dbId, ui] of this.listIdMap) {
        if (ui === uiId) return Number(dbId);
      }
      return null;
    }

    private getNumericCardId(cardId: string): number | null {
      const last = String(cardId).split(':').pop();
      const n = Number(last);
      return Number.isNaN(n) ? null : n;
    }

    moveCard(card: KanbanCardType, targetListId: string, sourceListId: string) {
      if (!card?.id) return;

        const itemId = this.getNumericCardId(card.id);
        const dbTargetListId = this.uiToDbListId(targetListId);

        if (itemId == null || dbTargetListId == null) {
          console.warn('Ids inválidos para mover tarjeta', { itemId, dbTargetListId });
          return;
        }

        const prev = this._lists.map(l => ({ ...l, cards: [...(l.cards ?? [])] }));

        const next = this._lists.map(l => {
          if (l.listId === sourceListId) {
            return { ...l, cards: (l.cards ?? []).filter(c => c.id !== card.id) };
          }
          if (l.listId === targetListId) {
            return { ...l, cards: [...(l.cards ?? []), { ...card }] };
          }
          return l;
        });
        this.updateLists(next);
        const payload: MoverItemDto = { itemId, listId: dbTargetListId };
        this.http
          .post(`${this.baseUrl}/catalog/moverItem`, payload)
          .subscribe({
            next: () => { console.log("Se envio") },
            error: err => {
              console.error('Error moviendo tarjeta', err);
              // rollback
              this.updateLists(prev);
            }
          });
    }

    onCardSelect(card: KanbanCardType, listId: string) {
        this.selectedCard.next(card);
        this.selectedListId.next(listId);
    }

    generateId() {
        let text = '';
        let possible = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';

        for (var i = 0; i < 5; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }

        return text;
    }

    isMobileDevice() {
        return /iPad|iPhone|iPod/.test(navigator.userAgent) || /(android)/i.test(navigator.userAgent);
    }
}
