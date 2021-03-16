import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { ProjectService } from './project.service';
import { NotificationService } from './shared/notifications/notification.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss'],
})
export class AppComponent {
	@ViewChild('toggleButton') toggleButton!: ElementRef;
	@ViewChild('menu') menu!: ElementRef;

	isMenuOpen = false;

	constructor(private renderer: Renderer2, private projectService: ProjectService, private notify: NotificationService) {
		this.renderer.listen('window', 'click', (e: Event) => {
			if (!this.toggleButton.nativeElement.contains(e.target) && !this.menu?.nativeElement.contains(e.target)) {
				this.isMenuOpen = false;
			}
		});
	}

	async onSave(): Promise<void> {
		try {
			await this.projectService.saveProject();
			this.isMenuOpen = false;
		} catch (error) {
			this.notify.error('Could not save project on server');
		}
	}
}
