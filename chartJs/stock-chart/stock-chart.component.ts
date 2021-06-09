import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { StockPrice } from 'src/app/models/stock-price';
import { StockPriceService } from 'src/app/services/stock-price.service';

import { AppConfigService } from 'src/app/services/app-config.service';
import { AppConfig } from 'src/app/models/app-config';
import * as moment from 'moment';

@Component({
  selector: 'app-stock-chart',
  templateUrl: './stock-chart.component.html',
  styleUrls: ['./stock-chart.component.css'],
  providers: [MessageService],
})
export class StockChartComponent implements OnInit, OnDestroy {
  stockPrices: StockPrice[] = [];
  rowGroupMetadata: any;
  stockName: string = '';
  loading: boolean = false;
  isChart: boolean = true;

  destroy$: Subject<boolean> = new Subject<boolean>();

  chartData: any;
  chartOptions: any;
  subscription: Subscription;
  config: AppConfig;

  constructor(
  private stockChartService: StockPriceService,
  private messageService: MessageService,
  private primengConfig: PrimeNGConfig,
  public router: Router,
  private route: ActivatedRoute,
  private configService: AppConfigService
  ) {
  this.chartData = {
    labels: [],
    datasets: [
    {
      type: 'line',
      label: 'Price',
      borderColor: '#42A5F5',
      borderWidth: 2,
      fill: true,
      data: [],
    },
    {
      type: 'bar',
      label: 'Delivery',
      backgroundColor: '#66BB6A',
      data: [],
      borderColor: 'white',
      borderWidth: 2,
    },
    ],
  };
  this.chartOptions = {
    responsive: true,
    title: {
    display: true,
    text: 'Combo Bar Line Chart',
    },
    tooltips: {
    mode: 'index',
    intersect: true,
    },
  };

  this.config = this.configService.config;
  this.updateChartOptions();
  this.subscription = this.configService.configUpdate$.subscribe((config:any) => {
    this.config = config;
    this.updateChartOptions();
  });
  }

  ngOnInit(): void {
  this.route.params.subscribe((params) => {
    this.stockName = params['stockName'];
    if (this.stockName) {
    this.searchRecords();
    }
  });
  this.primengConfig.ripple = true;
  }

  selectData(event: any) {
  //event.dataset = Selected dataset
  //event.element = Selected element
  //event.element._datasetIndex = Index of the dataset in data
  //event.element._index = Index of the data in dataset
  }

  updateChartOptions() {
  if (this.config.dark) this.applyDarkTheme();
  else this.applyLightTheme();
  }

  applyLightTheme() {
  this.chartOptions = {
    legend: {
    labels: {
      fontColor: '#495057',
    },
    },
    scales: {
    xAxes: [
      {
      ticks: {
        fontColor: '#495057',
      },
      },
    ],
    yAxes: [
      {
      ticks: {
        fontColor: '#495057',
      },
      },
    ],
    },
  };
  }

  applyDarkTheme() {
  this.chartOptions = {
    legend: {
    labels: {
      fontColor: '#ebedef',
    },
    },
    scales: {
    xAxes: [
      {
      ticks: {
        fontColor: '#ebedef',
      },
      gridLines: {
        color: 'rgba(255,255,255,0.2)',
      },
      },
    ],
    yAxes: [
      {
      ticks: {
        fontColor: '#ebedef',
      },
      gridLines: {
        color: 'rgba(255,255,255,0.2)',
      },
      },
    ],
    },
  };
  }

  ngOnDestroy() {
  this.destroy$.next(true);
  this.destroy$.unsubscribe();
  }

  searchRecords() {
  const key = 'stock_' + this.stockName;
  const cached = localStorage.getItem(key);
  if (!cached) {
    this.loading = true;
    this.stockChartService
    .getByName(this.stockName)
    .pipe(takeUntil(this.destroy$))
    .subscribe(
      (data: any[]) => {
      localStorage.setItem(key, JSON.stringify(data));
      this.stockPrices = data;
      this.loading = false;

      this.showHighChart();
      },
      (error) => {
      this.loading = false;
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Error in getting data.',
      });
      }
    );
  } else {
    this.stockPrices = JSON.parse(cached);
    this.showHighChart();
  }
  }

  private showHighChart() {
  if (this.isChart) {
    const ohlc:any = [];
    const volume = [];

    const close:any = [];
    const dates:any = [];
    const delVolume:any = [];

    this.stockPrices.forEach((row) => {
    ohlc.push([
      row.date,
      row.open,
      row.high,
      row.low,
      row.close, // close
    ]);

    volume.push([
      row.date,
      row.deliveryQty, // the volume
    ]);

    dates.push(moment(row.date).format('YYYY-MM-DD'));
    close.push(row.close);
    delVolume.push(row.deliveryQty);
    });

    if(dates.length > 0) {
      this.chartData.labels = dates;
      this.chartData.datasets[0].data = ohlc;
      //this.chartData.datasets[0].data = delVolume;
      this.chartData.datasets[1].data = delVolume;
    }

  }
  }

  clear() {
  const key = 'stock_' + this.stockName;
  localStorage.removeItem(key);
  this.stockPrices = [];
  }
}
