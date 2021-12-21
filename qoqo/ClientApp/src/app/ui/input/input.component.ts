import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.css'],
})
export class InputComponent implements OnInit {
  @ViewChild('input') input?: ElementRef;

  @Input('label') label = '';
  @Input('value') value = '';
  @Input('error') error = '';
  @Input('helper') helper = '';
  @Input('placeholder') placeholder = '';
  @Input('disabled') disabled = false;
  @Input('inputAttr') inputAttr = {};

  constructor() {}

  ngOnInit(): void {}

  ngOnChange() {}

  ngAfterViewInit() {
    Object.assign(this.input?.nativeElement, this.inputAttr);
  }

  public getValue() {
    return this.input?.nativeElement.value;
  }
}
