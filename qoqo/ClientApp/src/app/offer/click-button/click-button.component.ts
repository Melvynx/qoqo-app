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

@Component({
  selector: 'app-click-button',
  templateUrl: './click-button.component.html',
  styleUrls: ['./click-button.component.css'],
})
export class ClickButtonComponent implements OnInit {
  @ViewChild('next') next?: ElementRef;
  @ViewChild('current') curr?: ElementRef;
  @ViewChild('main') main?: ElementRef;

  @Input() disabled: boolean = false;
  @Input() loading: boolean = false;
  @Input() value: number = 0;
  @Output() onClick: EventEmitter<any> = new EventEmitter();

  localValue: number = 0;

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
    if (changes?.value?.previousValue) {
      this.handleNewValueAnimation(changes.value.currentValue);
    }
  }

  handleClick() {
    this.onClick.emit();
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
