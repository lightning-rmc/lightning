import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Layer } from 'src/app/Project.type';
import { SubSink } from 'subsink';
import { EditService } from '../edit.service';

@Component({
	templateUrl: './layer-details.component.html',
	styleUrls: ['./layer-details.component.scss'],
})
export class LayerDetailsComponent implements OnInit {
	constructor(private editService: EditService, private route: ActivatedRoute) {}

	activeTab?: string;
	layer?: Layer;
	layerId?: string;

	sink = new SubSink();

	async ngOnInit(): Promise<void> {
		this.sink.add(
			this.route.params.subscribe(async (params) => {
				const { layerId } = params;
				this.layerId = layerId;
				await this.loadLayer(layerId);
			})
		);
	}

	async loadLayer(layerId: string): Promise<void> {
		this.layer = await this.editService.getLayer(layerId);
	}

	async onSourceChange(filename: string) {
		await this.editService.setLayerSource(this.layerId!, filename);
	}
}
