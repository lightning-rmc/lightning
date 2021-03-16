import { Component, OnInit } from '@angular/core';
import { LayerGroup } from '../Project.type';
import { EditService } from './edit.service';

@Component({
	selector: 'app-edit',
	templateUrl: './edit.component.html',
	styleUrls: ['./edit.component.scss'],
})
export class EditComponent implements OnInit {
	constructor(private edit: EditService) {}

	groups: LayerGroup[] = [];

	async ngOnInit(): Promise<void> {
		await this.fetchRenderTrees();
	}

	async fetchRenderTrees(): Promise<void> {
		this.groups = await this.edit.getRenderTrees();
	}

	async addLayerForGroup(groupId: string): Promise<void> {
		try {
			await this.edit.addLayerToGroup(groupId);
			await this.fetchRenderTrees();
		} catch (error) {
			console.error(error);
		}
	}

	async addGroup(): Promise<void> {
		try {
			await this.edit.addGroup();
			await this.fetchRenderTrees();
		} catch (error) {
			console.error(error);
		}
	}
}
