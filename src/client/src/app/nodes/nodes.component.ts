import { Component, OnDestroy, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { Node } from './models/Node.type';
import { NodesService } from './nodes.service';

@Component({
	selector: 'app-nodes',
	templateUrl: './nodes.component.html',
	styleUrls: ['./nodes.component.scss'],
})
export class NodesComponent implements OnInit, OnDestroy {
	constructor(private nodesService: NodesService) {}

	private subs = new SubSink();

	nodes!: Node[];
	selectedNode?: Node;

	async ngOnInit() {
		this.nodes = await this.nodesService.getNodes();

		// LIVE UPDATE SUBSCRIPTIONS
		this.subs.sink = this.nodesService.nodeStateChange$.subscribe(({ id, state }) => {
			const node = this.nodes?.find((n) => n.id === id);
			if (node) {
				node.state = state;
			}
		});
	}

	ngOnDestroy() {
		this.subs.unsubscribe();
	}
}
