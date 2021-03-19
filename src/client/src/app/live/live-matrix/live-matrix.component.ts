import { Component, OnDestroy, OnInit } from '@angular/core';
import { EditService } from 'src/app/edit/edit.service';
import { Layer, LayerGroup } from 'src/app/Project.type';
import { SubSink } from 'subsink';
import { LiveService } from '../live.service';

@Component({
	selector: 'app-live-matrix',
	templateUrl: './live-matrix.component.html',
	styleUrls: ['./live-matrix.component.scss'],
})
export class LiveMatrixComponent implements OnInit, OnDestroy {
	constructor(private edit: EditService, private live: LiveService) {}

	groups: (LayerGroup & { layers: (Layer & { isActive?: boolean })[] })[] = [];

	sink = new SubSink();

	async ngOnInit(): Promise<void> {
		this.groups = await this.edit.getRenderTrees();

		this.sink.sink = this.live.layerActivationChanged$.subscribe((update) => this.syncLayerActivation(update.layerId, update.isActive));
	}

	ngOnDestroy() {
		this.sink.unsubscribe();
	}

	private syncLayerActivation(layerId: string, isActive: boolean) {
		for (const group of this.groups) {
			for (const layer of group.layers) {
				layer.isActive = isActive;
			}
		}
	}

	public async setLayerActivation(layer: (Layer & { isActive?: boolean}), isActive: boolean) {
		await this.live.setLayerActivation(layer.id, isActive);
		layer.isActive = isActive;
	}
}
