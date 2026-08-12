import { Component, inject, output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Input } from '../../../shared/components/forms/input/input';
import { Subtitle } from '../../../shared/components/text/subtitle/subtitle';
import { Button } from '../../../shared/components/forms/button/button';
import { TodoService } from '../../../core/services/todo-service/todo-service';

@Component({
  selector: 'app-todo-form',
  imports: [Input, Subtitle, Button, ReactiveFormsModule],
  templateUrl: './todo-form.html',
  styleUrl: './todo-form.css',
})
export class TodoForm {
  private readonly formBuilder = inject(FormBuilder);
  private readonly todoService = inject(TodoService);

  todoAdd = output<TodoDto>();

  loading = signal(false);
  error = signal<string | null>(null);

  protected form = this.formBuilder.nonNullable.group({
    title: ['', Validators.required],
  });

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.todoService.create(this.form.getRawValue()).subscribe({
      next: (todoDto) => {
        this.todoAdd.emit(todoDto);
        this.form.reset();
      },
      error: (err) => {
        this.error.set(err.message);
        this.loading.set(false);
      },
    });
  }
}
