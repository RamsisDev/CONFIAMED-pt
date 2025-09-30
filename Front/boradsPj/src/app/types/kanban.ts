import { Kanban } from '../apps/kanban/index';
export interface KanbanCardType {
    id: string;
    title?: string;
    description?: string;
    progress?: number;
    assignees?: Assignee[] | null;
    attachments?: number;
    comments?: Comment[];
    startDate?: string | Date;
    dueDate?: string | Date;
    completed?: boolean;
    priority?: Object;
    taskList: TaskList;
}


export interface KanbanListType {
    listId: string;
    title?: string;
    cards: KanbanCardType[];
}

export interface KanbanListTypeMs {
  id: number,
  code: string,
  title: string,
  position: number
}

export interface KabanCardsDto {
    id: string;
    listId: number;
    titulo?: string;
    descripcion?: string;
    startDate?: string;
    dueDate?: string;
    severidad: number
    estadoId?: number;
    asignadoAUsername?: string | string[];
    attachments?: number;
    comments?: Comment[];
    completed?: boolean;
    creadoEn: Date;
    actualizadoEnL: Date;
}
export interface MoverItemDto {
  itemId: number,
  listId: number
}

export interface Comment {
    id?: string;
    name: string;
    image?: string;
    text: string;
}

export interface ListName {
    listId?: string;
    title: string;
}

export interface TaskList {
    id?: string;
    title: string;
    tasks: Task[];
}

export interface Task {
    text: string;
    completed: boolean;
}

export interface Assignee {
    name: string;
    image: string;
}

export interface SaveCardDto {
  listCode: string;
  titulo: string;
  descripcion: string;
  startDate: string;   // yyyy-MM-dd
  dueDate: string;     // yyyy-MM-dd
  severidad: number;
  asignadoAUsernameId: number;
  externalId: string;
}
