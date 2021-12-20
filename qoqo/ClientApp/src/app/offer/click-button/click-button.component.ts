import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-click-button',
  templateUrl: './click-button.component.html',
  styleUrls: ['./click-button.component.css'],
})
export class ClickButtonComponent implements OnInit {
  @Input() disabled: boolean = false;

  constructor() {}

  ngOnInit(): void {}
}
