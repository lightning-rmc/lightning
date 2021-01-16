import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MediaRoutingModule } from './media-routing.module';
import { MediaComponent } from './media.component';
import { NgxFileDropModule } from 'ngx-file-drop';

@NgModule({
	declarations: [MediaComponent],
	imports: [CommonModule, MediaRoutingModule, NgxFileDropModule],
})
export class MediaModule {}
