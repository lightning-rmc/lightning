import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MediaService } from './media.service';
import { Media } from './models/Media.type';

@Component({
	selector: 'app-media',
	templateUrl: './media.component.html',
	styleUrls: ['./media.component.scss'],
})
export class MediaComponent implements OnInit {
	constructor(private service: MediaService) {}

	@ViewChild('input')
	inputElement!: ElementRef;

	files?: FileList;
	media: Media[] = [];

	async ngOnInit() {
		await this.loadMedia();
	}

	async loadMedia() {
		this.media = await this.service.getAllMedia();
	}

	handleFileInput(files: FileList) {
		this.files = files;
	}

	async onSubmit() {
		if (this.files && this.files.length > 0) {
			const result = await this.service.uploadFiles(this.files);
			let overallSuccess = true;
			for (let file in result) {
				overallSuccess = overallSuccess && result[file].success;
			}
			console.log('overall-success:', overallSuccess);
			console.log(result);
			this.inputElement.nativeElement.value = '';
			await this.loadMedia();
		}
	}
}
