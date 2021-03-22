import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LiveRoutingModule } from './live-routing.module';
import { LiveComponent } from './live.component';
import { MatButtonModule } from '@angular/material/button';
import { LiveMatrixComponent } from './live-matrix/live-matrix.component';

@NgModule({
	declarations: [LiveComponent, LiveMatrixComponent],
	imports: [CommonModule, LiveRoutingModule, MatButtonModule],
})
export class LiveModule {}
