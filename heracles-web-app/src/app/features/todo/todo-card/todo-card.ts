import { Component, inject, input, output, signal } from '@angular/core';
import { Checkbox } from '../../../shared/components/forms/checkbox/checkbox';
import { Subtitle } from '../../../shared/components/text/subtitle/subtitle';
import { SmallText } from '../../../shared/components/text/small-text/small-text';
import { DatePipe } from '@angular/common';
import { TodoService } from '../../../core/services/todo-service/todo-service';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Input } from '../../../shared/components/forms/input/input';
import { Button } from '../../../shared/components/forms/button/button';

@Component({
  selector: 'app-todo-card',
  imports: [
    Checkbox,
    Subtitle,
    SmallText,
    DatePipe,
    FormsModule,
    Input,
    ReactiveFormsModule,
    Button,
  ],
  templateUrl: './todo-card.html',
  styleUrl: './todo-card.css',
})
export class TodoCard {
  private readonly todoService = inject(TodoService);
  private readonly formBuilder = inject(FormBuilder);

  todo = input.required<TodoDto>();
  protected todoChange = output<TodoDto>();

  editing = signal(false);
  loading = signal(false);

  form = this.formBuilder.nonNullable.group({
    title: ['', Validators.required],
  });

  toggleCompleted() {
    this.todoService.toggle(this.todo().id).subscribe((todo) => this.todoChange.emit(todo));
  }

  startEditing() {
    this.form.patchValue({
      title: this.todo().title,
    });

    this.editing.set(true);
  }

  stopEditing() {
    this.editing.set(false);
  }

  editTodo() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.todoService.edit(this.form.getRawValue(), this.todo().id).subscribe({
      next: (todoDto) => {
        this.todoChange.emit(todoDto);
        this.editing.set(false);
        this.form.reset();
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }
}
