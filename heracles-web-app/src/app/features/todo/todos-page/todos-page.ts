import { Component, inject, OnInit, signal } from '@angular/core';
import { TodoService } from '../../../core/services/todo-service/todo-service';
import { PageResult } from '../../../shared/types/PageResult';
import { Title } from '../../../shared/components/text/title/title';
import { TodoCard } from '../todo-card/todo-card';
import { TodoForm } from '../todo-form/todo-form';

@Component({
  selector: 'app-todo-page',
  imports: [Title, TodoCard, TodoForm],
  templateUrl: './todos-page.html',
  styleUrl: './todos-page.css',
})
export class TodosPage implements OnInit {
  private readonly todoService = inject(TodoService);

  pageResult = signal<PageResult<TodoDto> | null>(null);

  ngOnInit() {
    this.todoService
      .getAll({
        page: 1,
        pageSize: 10,
        completed: false,
        search: '',
        sortBy: 'title',
        sortOrder: 'desc',
      })
      .subscribe((pageResult) => {
        this.pageResult.set(pageResult);
      });
  }

  onTodoChange(updatedTodo: TodoDto) {
    const results = this.pageResult()?.results;
    if (!results) return;

    const index = results.findIndex((todo) => todo.id === updatedTodo.id);

    if (index !== -1) results[index] = updatedTodo;
  }

  onTodoAdd(todoDto: TodoDto) {
    const results = this.pageResult()?.results;
    if (!results) return;

    results.push(todoDto)
  }
}
