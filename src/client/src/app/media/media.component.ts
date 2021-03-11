import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NotificationService } from '../shared/notifications/notification.service';
import { MediaService } from './media.service';
import { Media } from './models/Media.type';

@Component({
	selector: 'app-media',
	templateUrl: './media.component.html',
	styleUrls: ['./media.component.scss'],
})
export class MediaComponent implements OnInit {
	constructor(private service: MediaService, private notify: NotificationService) {}

	@ViewChild('input')
	inputElement!: ElementRef;

	files?: FileList;
	media: Media[] = [];

	isLoading = false;

	async ngOnInit(): Promise<void> {
		await this.loadMedia();
	}

	async loadMedia(): Promise<void> {
		this.isLoading = true;
		try {
			this.media = await this.service.getAllMedia();
		} catch (error) {
			this.notify.error('Failed to fetch media');
		} finally {
			this.isLoading = false;
		}
	}

	handleFileInput(files: FileList): void {
		this.files = files;
	}

	async onSubmit(): Promise<void> {
		if (this.files && this.files.length > 0) {
			const result = await this.service.uploadFiles(this.files);
			let overallSuccess = true;
			for (const file in result) {
				if (result[file]) {
					overallSuccess = overallSuccess && result[file].success;
				}
			}
			console.log('overall-success:', overallSuccess);
			console.log(result);
			this.inputElement.nativeElement.value = '';
			await this.loadMedia();
		}
	}
}
