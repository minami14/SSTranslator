# SSTranslator
SSTranslator automatically detects and translates text from the image when taking a screenshot.

SSTranslatorはスクリーンショット撮影時に自動的に画像からテキストを検出して翻訳するアプリケーションです。

# Install
* Download and extract the SSTranslator zip file from [Release](https://github.com/minami14/SSTranslator/releases) or clone and build this repository.
* Create a GCP service account referring to [Getting Started with Authentication](https://cloud.google.com/docs/authentication/getting-started) and set the environment variable GOOGLE_APPLICATION_CREDENTIALS.


* [Release](https://github.com/minami14/SSTranslator/releases)からSSTranslatorをダウンロードして展開する、またはこのリポジトリをクローンしてビルドする。
* [Getting Started with Authentication](https://cloud.google.com/docs/authentication/getting-started)を参考にGCPのサービスアカウントを作成し，環境変数GOOGLE_APPLICATION_CREDENTIALSを設定する。

# Usage
* Launch SSTranslator and click the Brows button to select the folder where the screenshots will be saved.
* Select the target language from the combo box in the upper right.
* Uncheck the upper left checkbox if you don't want to play audio.
* When you take a screenshot, it is automatically translated and the result is played back as an audio.


* SSTranslator.exeを起動してスクリーンショットが保存されるフォルダを選択します。
* 右上のドロップダウンリストから翻訳先の言語を選択します。
* 翻訳結果の音声を再生したくない場合は左上のチェックボックスをオフにします。
* スクリーンショットを撮影すると自動的に翻訳されて結果がテキストボックスに表示され，音声で再生されます。

# Find bugs? or want to add features?
If you find a bug or have features that you want to add, please make an issue or pull request.

不具合を見つけた場合や追加してほしい機能などがある場合にはIssueかプルリクエストを作成してください。

# License
MIT License

SSTranslator uses the following library.

[google-cloud-dotnet](https://github.com/googleapis/google-cloud-dotnet/blob/master/LICENSE)
