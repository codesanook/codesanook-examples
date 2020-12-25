call plug#begin()
    Plug 'tpope/vim-surround'
    Plug 'vim-scripts/matchit.zip'
    Plug 'mattn/emmet-vim'
    Plug 'pprovost/vim-ps1'
	Plug 'scrooloose/nerdtree'
    Plug 'neoclide/coc.nvim', {'branch': 'release'}

    "choco install fzf -y
    "choco install ag -y 
    Plug 'junegunn/fzf', { 'dir': '~/.fzf', 'do': './install --no-bash' }
    Plug 'junegunn/fzf.vim'

    " TypeScript
    "Plug 'leafgarland/typescript-vim'
    "Plug 'quramy/tsuquyomi'

    Plug 'ervandew/supertab'

    Plug 'prettier/vim-prettier', {
      \ 'do': 'yarn install',
      \ 'for': ['javascript', 'typescript', 'css', 'less', 'scss', 'json', 'graphql', 'vue', 'yaml', 'html'] } 

    " Themes
	Plug 'morhetz/gruvbox'
    Plug 'dracula/vim', { 'as': 'dracula' }
    Plug 'tomasiser/vim-code-dark'
    Plug 'vim-airline/vim-airline'
    Plug 'tpope/vim-fugitive'
    Plug 'maxmellon/vim-jsx-pretty'

    Plug 'dyng/ctrlsf.vim'
    
    "https://github.com/OmniSharp/omnisharp-vim#configuration
    "Plug 'OmniSharp/omnisharp-vim'

    "Markdown
    Plug 'godlygeek/tabular'
    Plug 'plasticboy/vim-markdown'
    Plug 'iamcco/markdown-preview.nvim', { 'do': 'cd app & yarn install'  }

    Plug 'wannesm/wmgraphviz.vim'

    "Maximize vim when start
call plug#end()

" Theme
syntax on
colorscheme dracula
highlight Comment guifg=#8692b7

" standard vim options
set guifont=Consolas:h14:cANSI
set nocompatible
set backspace=indent,eol,start
set guioptions-=T
set tabstop=4
set shiftwidth=4
set expandtab
set ignorecase
set smartcase
set smarttab
set smartindent
set wildmenu
set incsearch
set lazyredraw
set ttyfast
set wrap
set t_Co=256
set visualbell
set visualbell t_vb=
set encoding=utf-8
set clipboard=unnamed
set ruler
set number
set relativenumber
set ofu=syntaxcomplete#Complete
set laststatus=2

" Configuration for each plugin
runtime! macros/matchit.vim

filetype plugin indent on
set listchars=tab:>\ ,trail:-,extends:>,precedes:<,nbsp:+
set formatoptions+=j " Delete comment character when joining commented lines

" Nerdtree
" map NERDTree toggle
map <C-n> :NERDTreeToggle<CR>

" Super Tab
"https://vim.fandom.com/wiki/Omni_completion_popup_menu
let g:SuperTabDefaultCompletionType = "context"
"https://github.com/ervandew/supertab/issues/142#issuecomment-68664745
let g:SuperTabCrMapping = 1

" TypeScript configuration
"set omnifunc=syntaxcomplete#Complete
"let g:tsuquyomi_completion_detail = 1
"autocmd FileType typescript setlocal balloonexpr=tsuquyomi#balloonexpr()

" Turn off preview windows
set completeopt-=preview
"autocmd FileType typescript setlocal completeopt+=menu,preview

" Running before saving async (vim 8+):
let g:prettier#autoformat = 0
autocmd BufWritePre *.js,*.jsx,*.mjs,*.ts,*.tsx,*.css,*.less,*.scss,*.json,*.graphql,*.vue,*.yaml,*.html PrettierAsync

" use tabs over spaces
" Prettier default: false
let g:prettier#config#use_tabs = 'false'

" number of spaces per indentation level
" Prettier default: 2
let g:prettier#config#tab_width = 4

" Use vim-prettier settings as a default, but allows .editorconfig, .prettierrc, etc. to override them. 
" https://github.com/prettier/vim-prettier/issues/146#issuecomment-427082716
" cli-override|file-override|prefer-file
let g:prettier#config#config_precedence = 'prefer-file'

" single quotes over double quotes
" Prettier default: false
let g:prettier#config#single_quote = 'true'

syntax on
colorscheme dracula
highlight Comment guifg=#8692b7
nmap <leader>f <Plug>CtrlSFPrompt

" https://github.com/junegunn/fzf.vim
map <c-p> :Files<CR>
let $FZF_DEFAULT_OPTS='--bind ctrl-j:down,ctrl-k:up'
let $FZF_DEFAULT_COMMAND = 'ag -g ""'

" Move up and down in autocomplete with <c-j> and <c-k>
inoremap <expr> <c-j> ("\<C-n>")
inoremap <expr> <c-k> ("\<C-p>")

"nnoremap <Leader>cf :OmniSharpCodeFormat<CR>

"Markdown
"Disable Folding
let g:vim_markdown_folding_disabled = 1

" Set to 1, the MarkdownPreview command can be used for all files,
" by default it can only be used in Markdown files.
let g:mkdp_command_for_global = 1

" disable swap file
set noswapfile

"Make Vim start as full screen
"https://vim.fandom.com/wiki/Maximize_or_set_initial_window_size
autocmd GUIEnter * simalt ~x

"Permanent "very magic" modeEdit
nnoremap / /\v
vnoremap / /\v
cnoremap s/ smagic/

" shift+r to move clipboard registry to registry r
map <s-r> :let @r=@*<cr>

" disable backspace in insert mode and no recusive, we want to use <c-h> <c-w> <c-u>
" nop => no operation
:inoremap <BS> <Nop>
:inoremap <Del> <Nop>

" Disable auto comment insertion 
"https://vim.fandom.com/wiki/Disable_automatic_comment_insertion
autocmd FileType * setlocal formatoptions-=c formatoptions-=r formatoptions-=o

" let coc install the extension if they are missing:
" This requires Python 2
" choco install python2
let g:coc_global_extensions=['coc-omnisharp', 'coc-tsserver', 'coc-powershell']

" Enable vim-repeat
" https://github.com/tpope/vim-repeat/issues/55
" http://vimcasts.org/episodes/creating-repeatable-mappings-with-repeat-vim/
" silent! call repeat#set("\<Plug>surround.vim", v:count)
" nnoremap <silent> <Plug>SurroundWordWithApostrophe ysw'<esc> 
"     \ :call repeat#set("\<Plug>SurroundWordWithApostrophe", v:count)<cr>
" nmap <Leader>'  <Plug>SurroundWordWithApostrophe
" Remap keys for gotos
nmap <silent> gd <Plug>(coc-definition)
nmap <silent> <c-]> <Plug>(coc-definition)
nmap <silent> gy <Plug>(coc-type-definition)
nmap <silent> gi <Plug>(coc-implementation)
nmap <silent> gr <Plug>(coc-references)
