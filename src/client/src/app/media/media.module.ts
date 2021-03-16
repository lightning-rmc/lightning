import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MediaRoutingModule } from './media-routing.module';
import { MediaComponent } from './media.component';
import { NgxFileDropModule } from 'ngx-file-drop';

import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { NgxFilesizeModule } from 'ngx-filesize';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
	declarations: [MediaComponent],
	imports: [CommonModule, MediaRoutingModule, NgxFileDropModule, MatButtonModule, MatCheckboxModule, NgxFilesizeModule, ReactiveFormsModule],
})
export class MediaModule {}
