import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss'],
})
export class AppComponent {

	@ViewChild('toggleButton') toggleButton!: ElementRef;
	@ViewChild('menu') menu!: ElementRef;

	constructor(private renderer: Renderer2) {
		this.renderer.listen('window', 'click', (e: Event) => {
			if (!this.toggleButton.nativeElement.contains(e.target) && !this.menu?.nativeElement.contains(e.target)) {
				this.isMenuOpen = false;
			}
		})
	}

	isMenuOpen = false;
}
