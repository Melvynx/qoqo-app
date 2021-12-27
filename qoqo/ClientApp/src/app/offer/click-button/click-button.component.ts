import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  SimpleChange,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import * as confetti from 'canvas-confetti';

@Component({
  selector: 'app-click-button',
  templateUrl: './click-button.component.html',
  styleUrls: ['./click-button.component.css'],
})
export class ClickButtonComponent implements OnInit {
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

  ngOnInit(): void {
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

  handleClick() {
    this.onClick.emit();

    // const canvas: { confetti: typeof confetti } & HTMLCanvasElement =
    //   this.canvas?.nativeElement;
    // canvas.confetti = confetti.create(canvas, {
    //   resize: true,
    // }) as typeof confetti;

    // canvas.confetti({
    //   particleCount: 100,
    //   spread: 60,
    //   drift: 0.1,
    //   origin: { y: 1 },
    // });
  }

  handleTransitionEnd() {
    this.localValue = this.value;
    this.main?.nativeElement.classList.remove('roll');
  }

  handleNewValueAnimation(newValue: number) {
    this.value = newValue;
    this.main?.nativeElement.classList.add('roll');
  }
}
