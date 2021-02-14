import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { EditComponent } from './edit.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { LayerDetailsComponent } from './layer-details/layer-details.component';

const routes: Routes = [
	{
		path: '',
		component: EditComponent,
		children: [
			{
				path: 'layers/:layerId',
				component: LayerDetailsComponent,
				children: [
					{
						path: 'transform',
					},
					{
						path: 'color',
					},
					{
						path: 'blend',
					},
				],
			},
			{
				path: 'groups/:groupId',
				component: GroupDetailsComponent
			}
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class EditRoutingModule {}
