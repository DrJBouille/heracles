export interface TodoQuery {
  page: number;
  pageSize: number;
  search?: string;
  completed?: boolean;
  sortBy: string;
  sortOrder: 'asc' | 'desc';
}
