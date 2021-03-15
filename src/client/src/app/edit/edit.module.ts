import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditRoutingModule } from './edit-routing.module';
import { EditComponent } from './edit.component';
import { LayerMatrixComponent } from './layer-matrix/layer-matrix.component';
import { LayerDetailsComponent } from './layer-details/layer-details.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { LayerSourceComponent } from './layer-details/layer-source/layer-source.component';
import { LayerTransformComponent } from './layer-details/layer-transform/layer-transform.component';
import { LayerColorComponent } from './layer-details/layer-color/layer-color.component';
import { LayerBlendComponent } from './layer-details/layer-blend/layer-blend.component';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
	declarations: [
		EditComponent,
		LayerMatrixComponent,
		LayerDetailsComponent,
		GroupDetailsComponent,
		LayerSourceComponent,
		LayerTransformComponent,
		LayerColorComponent,
		LayerBlendComponent,
	],
	imports: [CommonModule, EditRoutingModule, MatButtonModule, MatSelectModule],
})
export class EditModule {}
