import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.css'],
})
export class InputComponent implements OnInit {
  @ViewChild('input') input?: ElementRef;

  @Input('label') label: string = '';
  @Input('value') value: string = '';
  @Input('error') error: string = '';
  @Input('placeholder') placeholder: string = '';
  @Input('disabled') disabled: boolean = false;
  @Input('inputAttr') inputAttr: any = {};

  constructor() {}

  ngOnInit(): void {}

  ngOnChange() {}

  ngAfterViewInit() {
    Object.assign(this.input?.nativeElement, this.inputAttr);
  }

  getValue() {
    return this.input?.nativeElement.value;
  }
}
