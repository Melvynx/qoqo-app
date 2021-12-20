import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-click',
  templateUrl: './click.component.html',
  styleUrls: ['./click.component.css'],
})
export class ClickComponent implements OnInit {
  @Input() variant: ClickComponentVariant = 'enabled';

  sentence: string =
    'Avec un score de 22 click, @JeanMichel a fait le 722ème click. Il a donc fait 3% des clicks total!';

  clickCounter: number = 55;

  constructor() {}

  ngOnInit(): void {}

  handleTest() {
    this.clickCounter++;
    console.log('handle test');
  }
}

type ClickComponentVariant = 'disabled' | 'enabled' | 'wine' | 'lose';
