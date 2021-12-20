import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-click',
  templateUrl: './click.component.html',
  styleUrls: ['./click.component.css'],
})
export class ClickComponent implements OnInit {
  @Input() variant: ClickComponentVariant = 'enabled';

  clickCounter: number = 55;

  constructor() {}

  ngOnInit(): void {}

  handleTest() {
    this.clickCounter++;
    console.log('handle test');
  }
}

type ClickComponentVariant = 'disabled' | 'enabled' | 'wine' | 'lose';
