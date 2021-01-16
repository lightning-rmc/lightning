import { Component, OnInit } from '@angular/core';
import { Node } from './models/Node';
import { NodesService } from './nodes.service';

@Component({
	selector: 'app-nodes',
	templateUrl: './nodes.component.html',
	styleUrls: ['./nodes.component.scss'],
})
export class NodesComponent implements OnInit {

	constructor(private nodesService: NodesService) {}

	nodes?: Node[];

	async ngOnInit() {
		this.nodes = await this.nodesService.getNodes();
	}
}
