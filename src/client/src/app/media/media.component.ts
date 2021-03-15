import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { setMaxListeners } from 'process';
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
	media: (Media & { selected: boolean })[] = [];

	isLoading = false;

	async ngOnInit(): Promise<void> {
		await this.loadMedia();
	}

	isAllSelected(): boolean {
		return this.media?.length > 0 ? this.media.every((m) => m.selected) : false;
	}

	isSomeSelected(): boolean {
		return this.media.some((m) => m.selected);
	}

	onAllSelectChange(value: boolean): void {
		this.media.forEach((m) => (m.selected = value));
	}

	async loadMedia(useCache = true): Promise<void> {
		this.isLoading = true;
		try {
			const media = await this.service.getAllMedia(useCache);

			this.media = media.map((m) => ({
				...m,
				selected: false,
			}));
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

	async deleteSelected(): Promise<void> {
		const namesToDelete = this.media.filter(m => m.selected).map(m => m.name);
		for (const filename of namesToDelete) {
			await this.service.deleteMedia(filename);
		}

		await this.loadMedia();
	}
}
