import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MediaService } from 'src/app/media/media.service';
import { Media } from 'src/app/media/models/Media.type';
import { Layer } from 'src/app/Project.type';

@Component({
	selector: 'app-layer-source',
	templateUrl: './layer-source.component.html',
	styleUrls: ['./layer-source.component.scss'],
})
export class LayerSourceComponent implements OnInit {
	constructor(private mediaService: MediaService) {}

	@Input()
	layer!: Layer;

	media: Media[] = [];

	@Output()
	sourceChange = new EventEmitter<string>();

	async ngOnInit(): Promise<void> {
		this.media = await this.mediaService.getAllMedia();
	}

	onSelectionChange(filename: string): void {
		this.sourceChange.emit(filename);
	}
}
