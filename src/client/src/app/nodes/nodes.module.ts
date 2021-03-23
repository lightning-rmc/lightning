import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { NodesRoutingModule } from './nodes-routing.module';
import { NodesComponent } from './nodes.component';
import { NodeListComponent } from './node-list/node-list.component';
import { NodeDetailsComponent } from './node-details/node-details.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
	declarations: [NodesComponent, NodeListComponent, NodeDetailsComponent],
	imports: [CommonModule, NodesRoutingModule, MatInputModule, MatButtonModule],
})
export class NodesModule {}
