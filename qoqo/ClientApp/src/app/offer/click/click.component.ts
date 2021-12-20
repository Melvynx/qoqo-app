import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-click',
  templateUrl: './click.component.html',
  styleUrls: ['./click.component.css'],
})
export class ClickComponent implements OnInit {
  @Input() variant: ClickComponentVariant = 'enabled';

  constructor() {}

  ngOnInit(): void {}
}

type ClickComponentVariant = 'disabled' | 'enabled' | 'wine' | 'lose';
