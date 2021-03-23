import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { NotificationService } from '../shared/notifications/notification.service';
import { Node } from './models/Node.type';
import { NodesService } from './nodes.service';

@Component({
	selector: 'app-nodes',
	templateUrl: './nodes.component.html',
	styleUrls: ['./nodes.component.scss'],
})
export class NodesComponent implements OnInit, OnDestroy {
	constructor(private nodesService: NodesService, private notify: NotificationService) {}

	private subs = new SubSink();

	nodes!: Node[];
	selectedNode?: Node;

	isLoading = false;

	async fetchNodes() {
		this.nodes = await this.nodesService.getNodes();
	}

	async ngOnInit(): Promise<void> {
		this.isLoading = true;
		try {
			await this.fetchNodes();
		} catch (error) {
			this.notify.error('Could not fetch nodes data');
		} finally {
			this.isLoading = false;
		}

		// LIVE UPDATE SUBSCRIPTIONS
		this.subs.sink = this.nodesService.nodeStateChange$.subscribe(async (update) => {
			const node = this.nodes.find((n) => n.id === update.id);
			if (node) {
				node.state = update.state;
			}
		});
		this.subs.sink = this.nodesService.nodeConnected$.subscribe(async (nodeId) => {
			await this.fetchNodes();
		});
	}

	ngOnDestroy(): void {
		this.subs.unsubscribe();
	}
}
