import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { environment as env } from 'src/environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
	declarations: [AppComponent],
	imports: [BrowserModule, AppRoutingModule, HttpClientModule, BrowserAnimationsModule, SharedModule, MatButtonModule],
	providers: [],
	bootstrap: [AppComponent],
})
export class AppModule {}
