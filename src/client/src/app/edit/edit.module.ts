import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditRoutingModule } from './edit-routing.module';
import { EditComponent } from './edit.component';
import { LayerMatrixComponent } from './layer-matrix/layer-matrix.component';
import { LayerDetailsComponent } from './layer-details/layer-details.component';
import { GroupDetailsComponent } from './group-details/group-details.component';

@NgModule({
	declarations: [EditComponent, LayerMatrixComponent, LayerDetailsComponent, GroupDetailsComponent],
	imports: [CommonModule, EditRoutingModule],
})
export class EditModule {}
