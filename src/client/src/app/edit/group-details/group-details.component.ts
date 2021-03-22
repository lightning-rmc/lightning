import { Component, OnInit } from '@angular/core';
import { NodesService } from 'src/app/nodes/nodes.service';
import { Node } from 'src/app/nodes/models/Node.type';
import { ActivatedRoute } from '@angular/router';
import { EditService } from '../edit.service';

@Component({
	templateUrl: './group-details.component.html',
	styleUrls: ['./group-details.component.scss'],
})
export class GroupDetailsComponent implements OnInit {
	constructor(private nodesService: NodesService, private route: ActivatedRoute, private edit: EditService) {}

	groupId?: string;
	nodes: Node[] = [];

	async ngOnInit() {
		this.route.params.subscribe(async params => {
			this.groupId = params.groupId;
			this.nodes = await this.nodesService.getNodes();
		});
	}
}
