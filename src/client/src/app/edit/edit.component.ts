import { Component, OnInit } from '@angular/core';
import { LayerGroup } from '../Project.type';
import { NotificationService } from '../shared/notifications/notification.service';
import { EditService } from './edit.service';

@Component({
	selector: 'app-edit',
	templateUrl: './edit.component.html',
	styleUrls: ['./edit.component.scss'],
})
export class EditComponent implements OnInit {
	constructor(private edit: EditService, private notify: NotificationService) {}

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

	async deleteLayer(layerId: string): Promise<void> {
		try {
			await this.edit.deleteLayer(layerId);
			await this.fetchRenderTrees();
		} catch (error) {
			this.notify.error('Could not delete layer: ' + error.message);
		}
	}

	async deleteGroup(groupId: string): Promise<void> {
		try {
			await this.edit.deleteGroup(groupId);
			await this.fetchRenderTrees();
		} catch (error) {
			this.notify.error('Could not delete group: ' + error.message);
		}
	}
}
