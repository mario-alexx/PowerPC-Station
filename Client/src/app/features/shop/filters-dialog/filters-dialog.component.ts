import { Component, inject } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})

/**
 * The FiltersDialogComponent handles the selection and application of brand and type filters 
 * for the shop product list. It is presented in a dialog, allowing the user to select the 
 * desired filters and apply them.
*/
export class FiltersDialogComponent {

  /** 
   * Injects ShopService to fetch brand and type data if necessary.
   * @private 
  */
  shopService = inject(ShopService);
  /** 
   * Injects MatDialogRef to manage the open/close behavior of the dialog.
   * @private 
  */
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  /** 
   * Injects MAT_DIALOG_DATA to retrieve the selected brands and types passed into the dialog.
   * @private 
  */
  data = inject(MAT_DIALOG_DATA);

  /** Stores the brands selected by the user. */
  selectedBrands: string[] = this.data.selectedBrands;
  /** Stores the types selected by the user. */
  selectedTypes: string[] = this.data.selectedTypes;

  /**
   * Closes the dialog and returns the selected filters (brands and types) to the calling component.
  */
  applyFilters(): void
  {
    this.dialogRef.close({
      selectedBrands: this.selectedBrands, 
      selectedTypes: this.selectedTypes
    })
  }
}
