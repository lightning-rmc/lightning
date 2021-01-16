import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NodesRoutingModule } from './nodes-routing.module';
import { NodesComponent } from './nodes.component';
import { NodeListComponent } from './node-list/node-list.component';
import { NodeDetailsComponent } from './node-details/node-details.component';

@NgModule({
	declarations: [NodesComponent, NodeListComponent, NodeDetailsComponent],
	imports: [CommonModule, NodesRoutingModule],
})
export class NodesModule {}
