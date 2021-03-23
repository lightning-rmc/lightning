import { Component, Input, OnInit } from '@angular/core';
import { NotificationService } from 'src/app/shared/notifications/notification.service';
import { Node } from '../models/Node.type';
import { NodeState } from '../models/NodeState.type';
import { NodesService } from '../nodes.service';

@Component({
	selector: 'app-node-details',
	templateUrl: './node-details.component.html',
	styleUrls: ['./node-details.component.scss'],
})
export class NodeDetailsComponent implements OnInit {
	@Input() node!: Node;

	constructor(private nodesService: NodesService, private notify: NotificationService) {}

	async ngOnInit() {}

	async setNodeName(name: string) {
		try {
			const node = await this.nodesService.setNodeName(this.node.id, name);
			this.node.name = node.name;
		} catch (error) {
			console.error('Could not set node name', error);
			this.notify.error('Could not set node name:\n' + error.message);
		}
	}
}
