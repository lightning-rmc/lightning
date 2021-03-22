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

		this.live.getLayerActivationUpdates().subscribe(({ layerId, isActive }) => {
			for (const group of this.groups) {
				for (const layer of group.layers) {
					if (layer.id === layerId) {
						layer.isActive = isActive;
					}
				}
			}
		});
	}

	ngOnDestroy() {
		this.sink.unsubscribe();
	}


	public async setLayerActivation(layer: (Layer & { isActive?: boolean}), isActive: boolean) {
		await this.live.setLayerActivation(layer.id, isActive);
	}
}
