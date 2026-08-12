export interface PageResult<T> {
  results: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
