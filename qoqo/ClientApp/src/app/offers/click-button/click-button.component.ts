import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import * as confetti from 'canvas-confetti';

@Component({
  selector: 'app-click-button',
  templateUrl: './click-button.component.html',
  styleUrls: ['./click-button.component.css'],
})
export class ClickButtonComponent implements OnInit, AfterViewInit {
  @ViewChild('next') next?: ElementRef;
  @ViewChild('current') curr?: ElementRef;
  @ViewChild('main') main?: ElementRef;
  @ViewChild('root') root?: ElementRef;
  @ViewChild('canvas') canvas?: ElementRef;

  @Input() disabled = false;
  @Input() loading = false;
  @Input() value = 0;
  @Output() onClick = new EventEmitter();

  localValue = 0;

  ngOnInit() {
    this.localValue = this.value;
  }

  ngAfterViewInit() {
    this.next?.nativeElement.addEventListener(
      'transitionend',
      this.handleTransitionEnd.bind(this)
    );
  }

  ngOnDestroy() {
    this.next?.nativeElement.removeEventListener(
      'transitionend',
      this.handleTransitionEnd.bind(this)
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes?.value?.previousValue !== undefined) {
      this.handleNewValueAnimation(changes.value.currentValue);
    }
  }

  handleClick(event: MouseEvent) {
    if (!event.isTrusted) return;
    this.onClick.emit();
  }

  runConfetti() {
    const canvas: { confetti: typeof confetti } & HTMLCanvasElement =
      this.canvas?.nativeElement;
    canvas.confetti = confetti.create(canvas, {
      resize: true,
    }) as typeof confetti;

    canvas.confetti({
      particleCount: 100,
      spread: 60,
      drift: 0.1,
      origin: { y: 1 },
    });
  }

  private handleTransitionEnd() {
    this.localValue = this.value;
    this.main?.nativeElement.classList.remove('roll');
  }

  private handleNewValueAnimation(newValue: number) {
    this.value = newValue;
    this.main?.nativeElement.classList.add('roll');
  }
}
