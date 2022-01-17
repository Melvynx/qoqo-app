import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
})
export class CounterComponent {
  public currentCount = 0;
  state: 'waiting' | 'running' | 'finished' = 'waiting';

  get isWaiting() {
    return this.state === 'waiting';
  }
  get isFinished() {
    return this.state === 'finished';
  }
  get isRunning() {
    return this.state === 'running';
  }

  public incrementCounter() {
    this.currentCount++;
  }
}
