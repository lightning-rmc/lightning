import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NodeDetailsComponent } from './node-details/node-details.component';

import { NodesComponent } from './nodes.component';

const routes: Routes = [
	{
		path: '',
		component: NodesComponent,
		children: [
			{
				path: ':node-id',
				component: NodeDetailsComponent
			}
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class NodesRoutingModule {}
