import { Component, HostListener, OnInit } from '@angular/core';
import { FileSystemFileEntry, NgxFileDropEntry } from 'ngx-file-drop';

@Component({
	selector: 'app-media',
	templateUrl: './media.component.html',
	styleUrls: ['./media.component.scss'],
})
export class MediaComponent implements OnInit {
	constructor() {}

	ngOnInit() {}

	async dropped(files: NgxFileDropEntry[]) {
		for (const droppedFile of files) {
			if (droppedFile.fileEntry.isFile) {
				const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
				const file = await this.readFileAsync(fileEntry);
				console.log(droppedFile.relativePath);
				const formData = new FormData();
				formData.append('file', file, droppedFile.relativePath);

				// submit form for file upload
			}
		}
	}

	private readFileAsync(file: FileSystemFileEntry): Promise<File> {
		return new Promise((resolve, reject) => {
			file.file((file) => {
				resolve(file);
			});
		});
	}
}
