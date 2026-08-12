import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TodoQuery } from '../../../shared/types/todo/TodoQuery';
import { PageResult } from '../../../shared/types/PageResult';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TodoService {
  private readonly apiUrl = `${environment.apiUrl}/todos`;

  private readonly http = inject(HttpClient);

  getAll(query: TodoQuery) {
    let params = new HttpParams()
      .set('page', query.page)
      .set('pageSize', query.pageSize)
      .set('sortBy', query.sortBy)
      .set('sortOrder', query.sortOrder);

    if (query.search) params = params.set('search', query.search);
    if (query.completed) params = params.set('isCompleted', query.completed);

    return this.http.get<PageResult<TodoDto>>(this.apiUrl, { params });
  }

  toggle(id: number) {
    return this.http.patch<TodoDto>(`${this.apiUrl}/${id}`, {});
  }

  create(request: CreateTodoRequestDto) {
    return this.http.post<TodoDto>(`${this.apiUrl}`, request);
  }

  edit(request: EditTodoRequestDto, id: number) {
    return this.http.put<TodoDto>(`${this.apiUrl}/${id}`, request);
  }
}
