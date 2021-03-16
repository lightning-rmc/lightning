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

	async ngOnInit(): Promise<void> {
		this.isLoading = true;
		try {
			this.nodes = await this.nodesService.getNodes();
		} catch (error) {
			this.notify.error('Could not fetch nodes data');
		} finally {
			this.isLoading = false;
		}

		// LIVE UPDATE SUBSCRIPTIONS
		this.subs.sink = this.nodesService.nodeStateChange$.subscribe({
			next: (update) => {
				console.log('got node state update', update);
				const node = this.nodes.find(n => n.id === update.id);
				if (node) {
					node.state = update.state;
				}
			},
		});
	}

	ngOnDestroy(): void {
		this.subs.unsubscribe();
	}
}
