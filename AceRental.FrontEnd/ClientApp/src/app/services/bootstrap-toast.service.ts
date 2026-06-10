import { Injectable } from '@angular/core';

export type BootstrapToastType = 'success' | 'error' | 'info' | 'warning';

@Injectable({
  providedIn: 'root'
})
export class BootstrapToastService {
  private container: HTMLElement | null = null;

  private getContainer(): HTMLElement {
    if (!this.container) {
      this.container = document.createElement('div');
      this.container.className = 'toast-container position-fixed top-0 end-0 p-3';
      this.container.style.zIndex = '1080';
      document.body.appendChild(this.container);
    }
    return this.container;
  }

  show(type: BootstrapToastType, header: string, body: string, duration = 5000) {
    const toast = document.createElement('div');
    const toastTypeClass = type === 'success' ? 'bg-success text-white' : type === 'warning' ? 'bg-warning text-black' : 'bg-danger text-white';
    toast.className = `toast show align-items-center border-0 ${toastTypeClass}`;
    toast.style.minWidth = '200px';
    toast.style.marginBottom = '0.75rem';
    toast.role = 'alert';
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');

    toast.innerHTML = `
      <div class="d-flex flex-column p-3">
        <div class="toast-header ${toastTypeClass} border-0 px-2 py-1 rounded-top">
          <strong class="me-auto">${header}</strong>
          <button type="button" class="btn-close btn-close-white me-2 m-auto" aria-label="Close"></button>
        </div>
        <div class="toast-body ${toastTypeClass} px-2 py-2">
          ${body}
        </div>
      </div>
    `;

    const closeButton = toast.querySelector('.btn-close');
    if (closeButton) {
      closeButton.addEventListener('click', () => this.dismiss(toast));
    }

    this.getContainer().appendChild(toast);
    setTimeout(() => this.dismiss(toast), duration);
  }

  success(header: string, body: string, duration = 5000) {
    this.show('success', header, body, duration);
  }

  error(header: string, body: string, status: number, duration = 5000) {
    if (status === 409) {
      this.show('warning', header, body, duration);
    }
    else {
      this.show('error', header, body, duration);
    }
  }


  private dismiss(toast: HTMLElement) {
    if (toast.parentElement) {
      toast.parentElement.removeChild(toast);
    }
  }
}
