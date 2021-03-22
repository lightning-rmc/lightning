import { Component, OnDestroy, OnInit } from '@angular/core';
import { NodesService } from 'src/app/nodes/nodes.service';
import { Node } from 'src/app/nodes/models/Node.type';
import { ActivatedRoute } from '@angular/router';
import { EditService } from '../edit.service';
import { SubSink } from 'subsink';

@Component({
	templateUrl: './group-details.component.html',
	styleUrls: ['./group-details.component.scss'],
})
export class GroupDetailsComponent implements OnInit, OnDestroy {
	constructor(private nodesService: NodesService, private route: ActivatedRoute, private edit: EditService) {}

	groupId: string = '';
	nodes: Node[] = [];

	sink = new SubSink();

	async ngOnInit() {
		this.sink.sink = this.route.params.subscribe(async (params) => {
			this.groupId = params.groupId;
			this.edit.getRenderTrees(this.groupId);
			this.nodes = await this.nodesService.getNodes();
		});
	}

	ngOnDestroy(): void {
		this.sink.unsubscribe();
	}
}
