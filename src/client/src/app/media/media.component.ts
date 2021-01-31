import { Component, OnInit } from '@angular/core';
import { MediaService } from './media.service';

@Component({
	selector: 'app-media',
	templateUrl: './media.component.html',
	styleUrls: ['./media.component.scss'],
})
export class MediaComponent implements OnInit {
	constructor(private service: MediaService) {}

	files?: FileList;

	ngOnInit() {}

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
		}
	}
}
