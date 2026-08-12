import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { SmallError } from '../small-error/small-error';

@Component({
  selector: 'app-input',
  imports: [ReactiveFormsModule, SmallError],
  templateUrl: './input.html',
  styleUrl: './input.css',
})
export class Input {
  label = input<string>('');
  placeholder = input<string>('');
  type = input<string>('text');
  control = input.required<FormControl>();
}
