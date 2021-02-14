import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { EditComponent } from './edit.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { LayerBlendComponent } from './layer-details/layer-blend/layer-blend.component';
import { LayerColorComponent } from './layer-details/layer-color/layer-color.component';
import { LayerDetailsComponent } from './layer-details/layer-details.component';
import { LayerSourceComponent } from './layer-details/layer-source/layer-source.component';
import { LayerTransformComponent } from './layer-details/layer-transform/layer-transform.component';

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
						path: 'source',
						component: LayerSourceComponent,
					},
					{
						path: 'transform',
						component: LayerTransformComponent,
					},
					{
						path: 'color',
						component: LayerColorComponent,
					},
					{
						path: 'blend',
						component: LayerBlendComponent,
					},
				],
			},
			{
				path: 'groups/:groupId',
				component: GroupDetailsComponent,
			},
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class EditRoutingModule {}
